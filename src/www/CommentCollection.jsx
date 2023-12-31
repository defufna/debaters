import { Component } from 'preact';
import { fromBase62 } from './utils.js';
import { Comment } from './Comment.jsx';
import { VoteBox } from './VoteBox.jsx';
import { SubmitComment } from './SubmitComment.jsx';

function buildTree(rootId, comments) {
    const commentMap = {}
    for (let i = 0; i < comments.length; i++) {
        const comment = comments[i];
        commentMap[comment.id] = comment;
    }

    const result = [];

    for (let i = 0; i < comments.length; i++) {
        const comment = comments[i];
        comment.score = comment.upvotes - comment.downvotes;

        if (typeof comment.children === 'undefined') {
            comment.children = []
        }

        if (comment.parent === rootId) {
            result.push(comment);
        } else {
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
        for (let i = 0; i < current.length; i++) {
            const child = current[i].children;
            if (child.length > 0)
                queue.push(child)
        }
    }

    return result;
}

export class CommentCollection extends Component {
    constructor(props) {
        super(props);
        this.onSubmitCommentDone = this.onSubmitCommentDone.bind(this);
    }

    async componentDidMount() {
        let { community, id, fetch } = this.props;
        id = fromBase62(id);

        try {
            const data = await fetch(`/api/Debate/GetComments?postId=${id}`);

            if (data.code === 0 && data.post.community.toLowerCase() === community.toLowerCase()) {
                this.setState({ comments: buildTree(data.post.id, data.comments), post: data.post, changeCounter:0 });
            } else {
                this.setState({ error: "Error fetching data." });
            }
        } catch (error) {
            this.setState({ error: error });
        }
    }

    onSubmitCommentDone = (success, result) => {
        if (success) {
            const newComments = [...this.state.comments];
            newComments.unshift(result);
            this.setState({ comments: newComments, changeCounter:this.state.changeCounter + 1})
        }
    }

    render({ fetch }) {
        let { error = null, post = null, comments = null, changeCounter } = this.state;

        if (error !== null) {
            return <p class="error">{error}</p>;
        }

        if (post === null) {
            return;
        }
        return (
            <div class="comments">
                <VoteBox fetch={fetch} node={post}></VoteBox>
                <h1>{post.title}</h1>
                <p><a href={`/c/${post.community}`}>{post.community}</a></p>
                <p>{post.content}</p>
                <p>{post.author}</p>
                <SubmitComment key={changeCounter} parent={post} fetch={fetch} onDone={this.onSubmitCommentDone} />
                {comments.map(c => <Comment key={c.id} comment={c} depth="0" fetch={fetch} />)}
            </div>
        );
    }
}

