import { fromBase62 } from './utils';
import { useState } from 'preact/hooks';


export function VoteBox({ node, fetch }) {
    const [nodeState, setNodeState] = useState(node);

    const vote = async (ev, upvote) => {
        ev.stopPropagation();

        if (nodeState.upvoted === 0) {
            const data = await fetch(`/api/Debate/Vote?nodeId=${node.id}&upvote=${upvote}`, {
                method: "POST",
            })
            if (data.code === 0) {
                let newNode = { ...nodeState };
                newNode.upvoted = upvote ? 1 : -1;
                if (upvote) {
                    newNode.upvotes += 1;
                } else {
                    newNode.downvotes += 1;
                }

                setNodeState(newNode);
            }
        } else {
            removeVote();
        }
    };

    async function removeVote() {
        const data = await fetch(`/api/Debate/RemoveVote?nodeId=${node.id}`, {
            method: "POST",
        })

        if (data.code === 0) {
            let newNode = { ...nodeState };
            newNode.upvoted = 0;
            if (nodeState.upvoted === 1) {
                newNode.upvotes -= 1;
            } else {
                newNode.downvotes -= 1;
            }
            setNodeState(newNode);
        }
    }

    return <div class="votebox">
        <button class={`up ${nodeState.upvoted === 1 ? 'voted' : ''}`} onClick={(ev) => vote(ev, true)}>▲</button>
        <button class={`down ${nodeState.upvoted === -1 ? 'voted' : ''}`} onClick={(ev) => vote(ev, false)}>▼</button>
        <span>{nodeState.upvotes - nodeState.downvotes}</span>
    </div>;
}
