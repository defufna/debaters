import { cloneElement } from 'preact';
import { useState } from 'preact/hooks';

export function Form({ onSubmit = (fd) => { }, children }) {
    const [formData, setFormData] = useState({});

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

    return (
        <form onSubmit={handleSubmit}>
            {children.map((child) => {
                return cloneElement(child, {
                    value: formData[child.props.name] || '',
                    onChange: handleChange,
                });
            })}
        </form>
    );
}
