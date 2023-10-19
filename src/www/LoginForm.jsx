import { login } from './LoginManager.jsx';
import { Form } from './Form.jsx';

export function LoginForm({ onDone = (r) => { }, onCancel = () => { } } ) {
    const handleSubmit = async (formData) => {
        try {
            const res = await login(formData.username, formData.password);
            return onDone(res);
        }
        catch (error) {
            console.log(error);
        }
        return null;
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
