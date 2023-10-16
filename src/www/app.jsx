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
            <header>
                <a id="logo" href="/">Debate.rs</a>
                <UserInfo />
            </header>
            <main>

            <Router>
                <PostCollection path="/" fetch={fetchWrapper} />
                <PostCollection path="/c/:community/" fetch={fetchWrapper} />
                <CommentCollection path="/c/:community/:id" fetch={fetchWrapper} />
                </Router>
            </main>
        </div>
    );
}

render(<App />, document.body);