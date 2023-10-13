import { Component } from 'preact';
import { route } from 'preact-router';
import { VoteBox } from './VoteBox';

export function Post({ post, fetch }) {
    let postUrl = `/c/${post.community}/${post.id62}`;
    return (
        <div class="post" onClick={() => route(postUrl)}>
            <VoteBox fetch={fetch} node={post}/>
            <div class="votetitle"><a href={postUrl}>{post.title}</a></div>
            <div class="votecommunity"><a href={`/c/${post.community}/`}>{post.community}</a></div>
        </div>
    );
}
