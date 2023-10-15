import { login } from './LoginManager.jsx';
import { Form } from './Form.jsx';

export function LoginForm({ onDone = (r) => { }, onCancel = () => { } } ) {
    const handleSubmit = (formData) => {
        login(formData.username, formData.password).then(res => {
            onDone(res);
        }).catch(error => console.log(error));
    };

    return (
        <div class="overlay">
            <div class="dialog">
                <Form onSubmit={handleSubmit}>
                    <label htmlFor="username">Username:</label>
                    <input
                        type="text"
                        id="username"
                        name="username"
                        required />
                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        required />
                    <div class="form-bottom-row">
                        <button type="submit">Login</button>
                        <button onClick={onCancel}>Cancel</button>
                    </div>
                </Form>
            </div>
        </div>
    );
}
