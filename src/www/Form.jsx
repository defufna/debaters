import { cloneElement } from 'preact';
import { useState } from 'preact/hooks';

export function Form({ onSubmit = (fd) => null, children }) {
    const [formData, setFormData] = useState({});
    const [message, setMessage] = useState(null);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const message = await onSubmit(formData);

        if (message) {
            setMessage(message);
        }
    };

    return (
        <div>
            {message && <p>{message}</p>}
            <form onSubmit={handleSubmit}>
                {children.map((child) => {
                    return cloneElement(child, {
                        value: formData[child.props.name] || '',
                        onChange: handleChange,
                    });
                })}
            </form>
        </div>
    );
}
