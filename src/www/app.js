import { html, Component } from 'https://esm.sh/htm/preact';
import { Router } from 'https://esm.sh/preact-router';
import { toBase62, fromBase62 } from '/utils.js';

class Post extends Component{
    render({ post })
    {
        return html`
            <p>
                ${post.upvotes - post.downvotes}
                <a href="/c/${post.community}/${post.id}">${post.title}</a>
                <a href="/c/${post.community}/">${post.community}</a>
            </p>`;
    }
}

function prepare(data) {
    for (let i = 0; i < data.length; i++) {
        data[i].id = toBase62(BigInt(data[i].id));
    }
    return data;
}

class PostCollection extends Component{
    componentDidMount() {
        fetch('/api/Debate/GetTopPosts')
          .then(response => response.json())
          .then(data => this.setState({ posts: prepare(data) }))
            .catch(error => {
                const msg = 'Error fetching data';
                console.error(msg, ": ", error)
                this.setState({error:msg})
            });
    }

    render({ }, { posts = [], error = null }) {

        if(error !== null){
            return html`<p class="error">${error}</p>`;
        }

        return html`
        <div>
            <h1>Top Posts</h1>
            ${posts.map(post => (
                html`<${Post} post=${post}/>`
            ))}
        </div>`;
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
            if (typeof child !== 'undefined' && child.length > 0)
                queue.push(child)
        }
    }

    return result;
}

class Comment extends Component{
    render({ comment, depth }){
        return html`
            <div class="comment" style="padding-left:${depth*24}px">
                <div>${comment.author}</div>
                <div>${comment.content}</div>
            </div>
        `;
    }
}

function renderComments(comments, depth, result) {
    for (let i = 0; i < comments.length; i++){
        const comment = comments[i];

        result.push(html`<${Comment} comment=${comment} depth=${depth} />`)

        if (typeof comment.children === 'undefined')
            continue;

        renderComments(comment.children, depth + 1, result);

    }
}

class CommentCollection extends Component {
    async componentDidMount(params) {
        let { community, id } = this.props;
        id = fromBase62(id);

        try {
            const response = await fetch(`/api/Debate/GetComments?postId=${id}`);
            const data = await response.json();

            if (data.code === 0) {
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
        console.log(this.state);
        if (post === null) {
            return;
        }
        if(error !== null){
            return html`<p class="error">${error}</p>`;
        }

        const renderedComments = [];
        renderComments(comments, 0, renderedComments);

        return html`
            <h1>${post.title}</h1>
            <p>${post.content}</p>
            ${renderedComments}
        `;
    }
}
export function App () {
    return html`
    <a href="/">Home</a>
    <${Router}>
        <${PostCollection} path="/"/>
        <${CommentCollection} path="/c/:community/:id"/>
    <//>`;
}
