import { Component } from "preact";
import { addLoginListener, logout } from "./LoginManager";
import { LoginForm } from "./LoginForm";
import { RegisterForm } from "./RegisterForm";

const Initial = 0;
const LoginVisible = 1;
const RegisterVisible = 2;

export class UserInfo extends Component {
	constructor(props) {
		super(props);

		this.state = { user: null, state:Initial, message:null };

		this.loginChanged = this.loginChanged.bind(this);
		this.logoutClicked = this.logoutClicked.bind(this);
		this.showLogin = this.showLogin.bind(this);
		this.showRegister = this.showRegister.bind(this);
		this.hideAll = this.hideAll.bind(this);
		this.registerDone = this.registerDone.bind(this);
		this.loginDone = this.loginDone.bind(this);

		addLoginListener(this.loginChanged);
	}

	showLogin() {
		this.setState({ state: LoginVisible });
	}

	showRegister() {
		this.setState({ state: RegisterVisible });
	}

	hideAll() {
		this.setState({ state: Initial });
	}

	registerDone(result) {
		var message = "Registration successful! Now, please log in using your username and password.";
		if (!result.success) {
			message = result.message;
		}
		this.setState({ message: message });
	}

	loginChanged(user) {
		const state = { user: user };
		if (user !== null) {
			state.state = Initial;
			state.message = null;
		}

		this.setState(state);
	}

	loginDone(result) {
		if (!result) {
			this.setState({ message: "Sorry, but it seems your username or password is incorrect. Please recheck your login credentials and give it another shot." });
		}
	}

	logoutClicked() {
		logout();
	}

	render({}, { user, state, message }) {
		if (user === null) {
			return (
				<div id="userinfo">
					{message && <span>{message}</span>}
					<button onClick={this.showRegister}>Register</button>
					<button href="#" onClick={this.showLogin}>Log in</button>
					{state === LoginVisible && <LoginForm onDone={this.loginDone} onCancel={this.hideAll} />}
					{state === RegisterVisible && <RegisterForm onDone={this.registerDone} onCancel={this.hideAll}/>}
				</div>
			);
		} else {
			return (
				<div id="userinfo">
					<span>{user.username}</span>
					<button onClick={this.logoutClicked}>Logout</button>
				</div>
			);
		}
	}
}
