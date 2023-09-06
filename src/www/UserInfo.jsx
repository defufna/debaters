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
		this.registerDone = this.registerDone.bind(this);

		addLoginListener(this.loginChanged);
	}

	showLogin() {
		this.setState({ state: LoginVisible });
	}

	showRegister() {
		this.setState({ state: RegisterVisible });
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

	logoutClicked() {
		logout();
	}

	render({}, { user, state, message }) {
		if (user === null) {
			return (
				<div>
					{message && <span>{message}</span>}
					<a href="#" onClick={this.showRegister}>Register</a>
					<a href="#" onClick={this.showLogin}>Log in</a>
					{state === LoginVisible && <LoginForm />}
					{state === RegisterVisible && <RegisterForm onDone={this.registerDone}/>}
				</div>
			);
		} else {
			return (
				<div>
					<span>{user.username}</span>
					<a href="#" onClick={this.logoutClicked}>Logout</a>
				</div>
			);
		}
	}
}
