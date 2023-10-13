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
                <button class="collapse-button" onClick={this.onCollapseClicked}>{collapsed ? "+" : "-"}</button>
                <div class="comment-header">
                    {!collapsed && <VoteBox fetch={fetch} node={comment}></VoteBox>}
                    <div class="comment-author">{comment.author}</div>
                </div>
                {!collapsed && <button class={`depth-${depth} collapse-bar`} onClick={this.onCollapseClicked} />}
                    <div class={`${collapsed?"collapsed":""} comment-container`}>
                        <div class="comment-content">{comment.content}</div>
                        {hasChildren &&
                            <div class="children">
                                {comment.children.map(c => <Comment comment={c} depth={depth.valueOf() + 1} fetch={fetch} />)}
                            </div>}
                    </div>
            </div>
        );
    }
}
