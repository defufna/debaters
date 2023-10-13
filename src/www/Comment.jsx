import { Component } from 'preact';
import { VoteBox } from './VoteBox';

export class Comment extends Component {
    constructor(props) {
        super(props);
        this.state = { collapsed: false };
        this.onCollapseClicked = this.onCollapseClicked.bind(this);
    }

    onCollapseClicked() {
        const { collapsed } = this.state;
        this.setState({ collapsed: !collapsed });
    }

    render({ comment, depth, fetch }) {
        let { collapsed = false } = this.state;
        const hasChildren = comment.children.length > 0;
        if (typeof depth === 'string') {
            depth = Number(depth);
        }

        return (
            <div class="comment" style={`padding-left:${Math.min(depth, 1) * 24}px`}>
                <a href="#" onClick={this.onCollapseClicked}>{collapsed ? "+" : "-"}</a>
                <VoteBox fetch={fetch} node={comment}></VoteBox>
                <div class="commentAuthor">{comment.author}</div>
                {!collapsed &&
                    <div class={`depth-${depth}`}>
                        <div class="commentContent">{comment.content}</div>
                        {hasChildren &&
                            <div class="children">
                                {comment.children.map(c => <Comment comment={c} depth={depth.valueOf() + 1} fetch={fetch} />)}
                            </div>}
                    </div>}
            </div>
        );
    }
}
