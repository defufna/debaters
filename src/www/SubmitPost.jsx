import { route } from "preact-router";
import { Form } from "./Form";
import { ResultCode } from "./LoginManager";
import { toBase62 } from "./utils";

export function SubmitPost({ community, fetch }) {
    const onSubmit = async ({title, content}) => {
        const data = await fetch(`/api/Debate/SubmitPost?communityName=${community}&title=${title}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(content)
        })

        if (data.code === ResultCode.Success) {
            route(`/c/${community}/${toBase62(BigInt(data.id))}`)
        }
    }
    return <div class="overlay">
        <div class="dialog submit-post">
            <h1>Submit post</h1>

            <Form onSubmit={onSubmit}>
                    <label htmlFor="title">Title:</label>
                    <input
                        type="text"
                        id="title"
                        name="title"
                        required />
                    <label htmlFor="password">Content:</label>
                    <textarea
                        id="content"
                        name="content"
                        required />
                    <div class="form-bottom-row">
                        <button type="submit">Submit</button>
                        <button onClick={()=>route(`/c/${community}`)}>Cancel</button>
                    </div>
            </Form>

        </div>
    </div>;
}
