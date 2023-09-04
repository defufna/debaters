import { render } from 'preact';
import { Router } from 'preact-router';
import { PostCollection } from './PostCollection.jsx';
import { CommentCollection } from './CommentCollection.jsx';
import { fetchWrapper } from './LoginManager.jsx';
import { UserInfo } from './UserInfo.jsx';
import { LoginForm } from './LoginForm.jsx';

export function App() {
    return (
        <div>
            <a href="/">Home</a>
            <UserInfo />
            <Router>
                <PostCollection path="/" fetch={fetchWrapper} />
                <PostCollection path="/c/:community/" fetch={fetchWrapper} />
                <CommentCollection path="/c/:community/:id" fetch={fetchWrapper} />
                <LoginForm path="/login" />
            </Router>
        </div>
    );
}

render(<App />, document.body);