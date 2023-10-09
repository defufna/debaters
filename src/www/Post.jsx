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
            <div class="votebox"><a class="up" href="#" onClick={(ev)=>vote(ev, true)}>▲</a> <a class="down" href="#" onClick={(ev)=>vote(ev, false)}>▼</a><span>{post.upvotes - post.downvotes}</span></div>
            <div class="votetitle"><a href={postUrl}>{post.title}</a></div>
            <div class="votecommunity"><a href={`/c/${post.community}/`}>{post.community}</a></div>
        </div>
    );
}
