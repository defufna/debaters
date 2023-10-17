import { Form } from './Form.jsx';
import { ResultCode, getUser } from './LoginManager.jsx';


export function SubmitComment({ parent, fetch, cancellable = false, onDone = (success, result) => { } }) {
    const onSubmit = async ({ content }) => {
        const data = await fetch(`/api/Debate/SubmitComment?parentId=${parent.id}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(content)
        });

        if (data.code === ResultCode.Success) {
            const newComment = {
                id: data.id, parent: parent, children: [], content: content, downvotes: 0, upvotes: 0,
                upvoted: 0, score: 0, author:getUser()
            }
            onDone(true, newComment);
        }
        else {
            onDone(false, "Error submiting comment");
        }
    };
    return <div class="submit-comment">
        <Form onSubmit={onSubmit}>
            <label htmlFor="content">Comment:</label>
            <textarea id="content" name="content"></textarea>
            <span>
            {cancellable && <button type="button">Cancel</button>}
                <button type="submit">Submit</button>
            </span>
        </Form>
    </div>;
}
