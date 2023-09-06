import { Form } from './Form.jsx';
import { register } from './LoginManager.jsx';


export function RegisterForm({ onDone = (r) => { } }) {
    const handleSubmit = (formData) => {
        register(formData.username, formData.password, formData.email).then((result) => {
            onDone(result);
        })
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
                    <label htmlFor="email">Email:</label>
                    <input
                        type="text"
                        id="email"
                        name="email"
                        required />
                </div>
                <div>
                    <button type="submit">Register</button>
                </div>
            </Form>
        </div>
    );
}
