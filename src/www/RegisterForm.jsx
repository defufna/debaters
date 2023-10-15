import { Form } from './Form.jsx';
import { register } from './LoginManager.jsx';


export function RegisterForm({ onDone = (r) => { }, onCancel = () => { } }) {
    const handleSubmit = (formData) => {
        register(formData.username, formData.password, formData.email).then((result) => {
            onDone(result);
        })
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
                    <label htmlFor="email">Email:</label>
                    <input
                        type="text"
                        id="email"
                        name="email"
                        required />
                    <div class="form-bottom-row">
                        <button type="submit">Register</button>
                        <button onClick={onCancel}>Cancel</button>
                    </div>
                </Form>
            </div>
        </div>
    );
}
