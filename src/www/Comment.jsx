import { Component } from 'preact';
import { VoteBox } from './VoteBox';
import { SubmitComment } from './SubmitComment';

export class Comment extends Component {
    constructor(props) {
        super(props);
        this.state = { collapsed: false, replyVisible: false, comment: props.comment };
        this.onCollapseClicked = this.onCollapseClicked.bind(this);
        this.onReplyClicked = this.onReplyClicked.bind(this);
        this.onSubmitCommentDone = this.onSubmitCommentDone.bind(this);
    }

    onCollapseClicked() {
        const { collapsed } = this.state;
        this.setState({ collapsed: !collapsed });
    }

    onReplyClicked() {
        const { replyVisible } = this.state;
        this.setState({ replyVisible: !replyVisible });
    }

    onSubmitCommentDone(success, result) {
        if (success) {
            const comment = { ...this.state.comment };
            comment.children.unshift(result);
            this.setState({ comment: comment, replyVisible:false });
        }
    }

    render({ depth, fetch }, { collapsed = false, replyVisible, comment }) {
        if (typeof comment === 'undefined') {
            return;
        }
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
                <div class={`${collapsed ? "collapsed" : ""} comment-container`}>
                    <div class="comment-content">{comment.content}</div>
                    {!replyVisible && <button onClick={this.onReplyClicked}>Reply</button>}
                    {replyVisible && <SubmitComment cancellable={true} fetch={fetch} parent={comment} onDone={this.onSubmitCommentDone} />}

                    {hasChildren &&
                        <div class="children">
                            {comment.children.map(c => <Comment key={c.id} comment={c} depth={depth.valueOf() + 1} fetch={fetch} />)}
                        </div>}
                </div>
            </div>
        );
    }
}
