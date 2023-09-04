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
}

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
    return fetch("/api/User/Login", { method: "POST", headers: {"Content-Type": "application/json"}, body: JSON.stringify({ username: username, password: password }) })
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

export function addLoginListener(callback) {
    callbacks.push(callback);
}