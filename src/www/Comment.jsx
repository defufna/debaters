import { Component } from 'preact';

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
                                {comment.children.map(c => <Comment comment={c} depth={depth.valueOf() + 1} />)}
                            </div>}
                    </div>}
            </div>
        );
    }
}
