import { Component } from 'preact';


export class Post extends Component {
    render({ post }) {
        return (
            <p>
                {post.upvotes - post.downvotes}
                <a href={`/c/${post.community}/${post.id}`}>{post.title}</a>
                <a href={`/c/${post.community}/`}>{post.community}</a>
            </p>
        );
    }
}
