import { Component } from 'preact';
import { route } from 'preact-router';
import { fromBase62 } from './utils';

export function Post({ post, fetch }) {
    let postUrl = `/c/${post.community}/${post.id}`;
    const vote = (ev, upvote) => {
        fetch(`/api/Debate/Vote?nodeId=${fromBase62(post.id)}&upvote=${upvote}`, {
            method: "POST",
        })
        .then((response) => response.json())
        .then((data) => {
        });

        ev.stopPropagation();
    }
    return (
        <div class="post" onClick={() => route(postUrl)}>
            <div class="votebox"><button class={`up ${post.upvoted === 1?'voted':''}`} onClick={(ev)=>vote(ev, true)}>▲</button> <button class={`down ${post.upvoted === -1?'voted':''}`} onClick={(ev)=>vote(ev, false)}>▼</button><span>{post.upvotes - post.downvotes}</span></div>
            <div class="votetitle"><a href={postUrl}>{post.title}</a></div>
            <div class="votecommunity"><a href={`/c/${post.community}/`}>{post.community}</a></div>
        </div>
    );
}
