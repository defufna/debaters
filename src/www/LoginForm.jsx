import { login } from './LoginManager.jsx';
import { Form } from './Form.jsx';

export function LoginForm({ onDone = (r) => { } }) {
    const handleSubmit = (formData) => {
        login(formData.username, formData.password).then(res => {
            onDone(res);
        }).catch(error => console.log(error));
    };

    return (
        <div>
            <Form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="username">Username:</label>
                    <input
                        type="text"
                        id="username"
                        name="username"
                        required />
                </div>
                <div>
                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        required />
                </div>
                <div>
                    <button type="submit">Login</button>
                </div>
            </Form>
        </div>
    );
}
