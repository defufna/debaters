import { Component, render } from 'preact';
import { Router } from 'preact-router';
import { toBase62, fromBase62 } from './utils.js';

class Post extends Component{
    render({ post })
    {
        return (
            <p>
                {post.upvotes - post.downvotes}
                <a href={`/c/${post.community}/${post.id}`}>{post.title}</a>
                <a href={`/c/${post.community}/`}>{post.community}</a>
            </p>
        );
    }
}

function prepare(data) {
    for (let i = 0; i < data.posts.length; i++) {
        data.posts[i].id = toBase62(BigInt(data.posts[i].id));
    }
    return data.posts;
}

class PostCollection extends Component{
    componentDidMount() {
        this.fetchData();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.community !== this.props.community) {
            this.fetchData();
        }
    }

    fetchData() {
        let { community = null } = this.props;
        let url = (community === null) ? "/api/Debate/GetTopPosts" : `/api/Debate/GetTopPosts?communityName=${community}`;
        fetch(url)
          .then(response => response.json())
            .then(data => {
                if (data.code === 0) {
                    this.setState({ posts: prepare(data), error:null })
                }
                else {
                    if (data.code === 4) {
                        this.setState({ error: `Error fetching data, invalid community "${community}"`, data:null });
                    } else {
                        this.setState({ error: `Error fetching data`, data:null });
                    }
                }
            })
            .catch(error => {
                const msg = 'Error fetching data';
                console.error(msg, ": ", error)
                this.setState({error:msg})
            });
    }

    render({ community=null }, { posts = [], error = null }) {
        if(error !== null){
            return (<p class="error">{error}</p>);
        }

        let heading = (community === null) ? "Top Posts" : community;

        return (
        <div>
            <h1>{heading}</h1>
            {posts.map(post => (
                (<Post post={post}/>)
            ))}
        </div>);
    }
}

function buildTree(rootId, comments) {
    const commentMap = {}
    for (let i = 0; i < comments.length; i++){
        const comment = comments[i];
        commentMap[comment.id] = comment;
    }

    const result = [];

    for (let i = 0; i < comments.length; i++){
        const comment = comments[i];
        comment.score = comment.upvotes - comment.downvotes;

        if (typeof comment.children === 'undefined') {
            comment.children = []
        }

        if (comment.parent === rootId) {
            result.push(comment);
        } else
        {
            const parent = commentMap[comment.parent];

            if (typeof parent.children === 'undefined') {
                parent.children = []
            }

            parent.children.push(comment);
        }
    }

    const queue = [result]

    while (queue.length > 0) {
        const current = queue.pop();
        current.sort((a, b) => b.score - a.score);
        for (let i = 0; i < current.length; i++){
            const child = current[i].children;
            if (child.length > 0)
                queue.push(child)
        }
    }

    return result;
}

class Comment extends Component{
    constructor(props) {
        super(props);
        this.state = { collapsed: false }
        this.onCollapseClicked = this.onCollapseClicked.bind(this);
    }

    onCollapseClicked() {
        const { collapsed } = this.state;
        this.setState({collapsed:!collapsed})
    }

    render({ comment, depth }) {
        let { collapsed = false } = this.state;
        const hasChildren = comment.children.length > 0;
        if (typeof depth === 'string') {
            depth = Number(depth);
        }

        return (
            <div class="comment" style={`padding-left:${Math.min(depth, 1) * 24}px`}>
                <a href="#" onclick={this.onCollapseClicked}>{collapsed ? "+" : "-"}</a>
                <div class="commentAuthor">{comment.author}</div>
                {!collapsed && 
                <div class={`depth-${depth}`}>
                    <div class="commentContent">{comment.content}</div>
                    {hasChildren && 
                    <div class="children">
                        {comment.children.map(c=><Comment comment={c} depth={depth.valueOf()+1}/>)}
                    </div>
                }
                </div>
                }
            </div>
        );
    }
}

class CommentCollection extends Component {
    async componentDidMount() {
        let { community, id } = this.props;
        id = fromBase62(id);

        try {
            const response = await fetch(`/api/Debate/GetComments?postId=${id}`);
            const data = await response.json();

            if (data.code === 0 && data.post.community.toLowerCase() === community.toLowerCase()) {
                this.setState({ comments: buildTree(data.post.id, data.comments), post: data.post });
            } else {
                this.setState({ error: "Error fetching data." });
            }
        } catch (error)
        {
            this.setState({ error: error });
        }
    }

    render()
    {
        let { error = null, post = null, comments = null } = this.state;

        if (error !== null) {
            return <p class="error">{error}</p>;
        }

        if (post === null) {
            return;
        }

        return (
            <div>
                <h1>{post.title}</h1>
                <p>{post.content}</p>
                {comments.map(c => <Comment comment={c} depth="0" />)}
            </div>
        );
    }
}

export function App () {
    return (
        <div>
            <a href="/">Home</a>
            <Router>
                <PostCollection path="/" />
                <PostCollection path="/c/:community/" />
                <CommentCollection path="/c/:community/:id" />
            </Router>
        </div>
    );
}

render(<App/>, document.body);