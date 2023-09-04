import { Component } from 'preact';
import { fromBase62 } from './utils.js';
import { Comment } from './Comment.jsx';

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

export class CommentCollection extends Component {
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
        } catch (error) {
            this.setState({ error: error });
        }
    }

    render() {
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
