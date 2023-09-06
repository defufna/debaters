const callbacks = [];

const ResultCode = {
    Success: 0,
    UnknownError: 1,
    InvalidSession: 2,
    InvalidName: 3,
    InvalidCommunity: 4,
    InvalidTitle: 5,
    InvalidContent: 6,
    InvalidPost: 7,
    InvalidParent: 8,
    InvalidCommentOrPost: 9,
    InvalidPassword: 10,
    InvalidEmail: 11,
    InvalidUsername: 12,
    AlreadyExists: 13,
    DoesNotExist: 14,
};

const ResultCodeMessages = [
    "Success", "Unknown Error", "Invalid Session", "Invalid username",
    "Invalid community", "Invalid title, title should be longer than 10 characters",
    "Invalid content, content should be at least 10 characters long",
    "Invalid post",
    "Invalid parent",
    "Invalid comment or post",
    "Your password is too weak. Please make it longer. You can include a mix of uppercase letters, lowercase letters, special characters, or numbers for added security",
    "Invalid email address",
    "Your username is invalid. Please use only letters and numbers in your username",
    "Already exists",
    "Does not exist"
];

var user = null;

export function fetchWrapper(url, options) {
    let promise = fetch(url, options).then((response) => response.json());
    promise.then(data => {
        if (data.code === undefined) {
            return;
        }

        if (data.code === ResultCode.InvalidSession) {
            updateUser(null);
            return;
        }

        if (data.user === undefined) {
            return;
        }

        updateUser(data.user);
    });
    return promise;
}

function updateUser(newUser) {
    if ((newUser === null) !== (user === null)) {
        user = newUser;
        for (let callback of callbacks) {
            callback(newUser);
        }
    }
}

export function logout() {
    return fetch("/api/User/LogOut", { method: "POST" }).then((response) => {
        if (response.ok) {
            updateUser(null);
        }
    });
}

export function login(username, password) {
    return fetch("/api/User/Login", { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify({ username: username, password: password }) })
        .then((response) => {
            if (response.ok) {
                updateUser({ username: username });
                return true;
            }
            else {
                return false;
            }
        });
}

export function register(username, password, email) {
    return fetch("/api/User/Register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username: username, password: password, eMail: email })
    })
        .then((response) => response.json())
        .then((data) => {
            return ({
                success: data.code === ResultCode.Success,
                code: data.code,
                message: ResultCodeMessages[data.code]
            })
        });
}

export function addLoginListener(callback) {
    callbacks.push(callback);
}