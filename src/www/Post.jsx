import { Component } from 'preact';
import { route } from 'preact-router';

export function Post({ post }) {
    let postUrl = `/c/${post.community}/${post.id}`;
    const vote = (ev, upvote) => {
        console.log(upvote, ev);
        ev.stopPropagation();
    }
    return (
        <div class="post" onClick={()=>route(postUrl)}>
            <div class="votebox"><button class="up" onClick={(ev)=>vote(ev, true)}>▲</button> <button class="down" onClick={(ev)=>vote(ev, false)}>▼</button><span>{post.upvotes - post.downvotes}</span></div>
            <div class="votetitle"><a href={postUrl}>{post.title}</a></div>
            <div class="votecommunity"><a href={`/c/${post.community}/`}>{post.community}</a></div>
        </div>
    );
}
