import { Component } from "preact";
import { addLoginListener, logout } from "./LoginManager";

export class UserInfo extends Component {
	constructor(props) {
		super(props);

		this.state = { user: null };

		this.onLogin = this.onLogin.bind(this);
		this.logout = this.onLogout.bind(this);
		addLoginListener(this.onLogin);
	}

	onLogin(user) {
		this.setState({ user: user });
	}

	onLogout() {
		logout();
	}

	render({ }, { user }) {
		if (user === null) {
			return (
				<div>
					<a href="/register">Register</a>
					<a href="/login">Log in</a>
				</div>
			);
		} else {
			return (
				<div>
					<span>{user.username}</span>
					<a href="#" onClick={this.onLogout}>Logout</a>
				</div>
			);
		}
	}
}
