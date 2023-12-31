import { Component } from 'preact';
import { Post } from './Post.jsx';
import { toBase62 } from './utils.js';
import { route } from 'preact-router';
import { SubmitPost } from './SubmitPost.jsx';
import { addLoginListener, isLoggedIn } from './LoginManager.jsx';

function prepare(data) {
    for (let i = 0; i < data.posts.length; i++) {
        data.posts[i].id62 = toBase62(BigInt(data.posts[i].id));
    }

    data.posts.sort((a, b) => (b.upvotes - b.downvotes) - (a.upvotes - a.downvotes));
    return data.posts;
}

export class PostCollection extends Component {
    constructor(props) {
        super(props);
        this.fetchData = this.fetchData.bind(this);
        this.loginChanged = this.loginChanged.bind(this);

        addLoginListener(this.loginChanged);
    }

    loginChanged(user) {
        const dataUser = this.state.user;
        console.log(dataUser);
        if (typeof dataUser === 'undefined')
            return;

        const dataUsername = (dataUser !== null) ? dataUser.username : null;
        const username = (user !== null) ? user.username : null;

        if (dataUsername !== username) {
            this.fetchData();
        }
    }

    componentDidMount() {
        this.fetchData();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.community !== this.props.community) {
            this.fetchData();
        }
    }

    fetchData() {
        let { community = null, fetch } = this.props;
        let url = (community === null) ? "/api/Debate/GetTopPosts" : `/api/Debate/GetTopPosts?communityName=${community}`;
        fetch(url)
            .then(data => {
                if (data.code === 0) {
                    this.setState({ posts: prepare(data), user: data.user, error: null });
                }
                else {
                    if (data.code === 4) {
                        this.setState({ error: `Error fetching data, invalid community "${community}"`, data: null, user: null });
                    } else {
                        this.setState({ error: `Error fetching data`, data: null, user: null });
                    }
                }
            })
            .catch(error => {
                const msg = 'Error fetching data';
                console.error(msg, ": ", error);
                this.setState({ error: msg });
            });
    }

    render({ community = null, fetch, submit }, { posts = [], user = null, error = null }) {
        if (error !== null) {
            return (<p class="error">{error}</p>);
        }

        submit = submit === "";

        const heading = (community === null) ? "Top Posts" : community;

        return (
            <div class="posts">
                <h1>{heading}</h1>
                {community !== null && isLoggedIn() &&
                    <div class="spanall">
                        <button onClick={() => route(`/c/${community}?submit`)}>Submit Post</button>
                    </div>
                }
                {submit &&
                    <SubmitPost community={community} fetch={fetch} />
                }
                {posts.map(post => (
                    (<Post post={post} fetch={fetch} />)
                ))}
            </div>);
    }
}
