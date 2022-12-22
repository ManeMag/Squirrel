import React from 'react';
import googleicon from "../../GoogleAuth.svg";
import abo from "../../or.svg";

const Authorization = () => {
    return (
        <form className="item login" name="form">
            <div className="items image">
                <img src={googleicon} alt="google"></img>
            </div>
            <div className="items or">
                <img src={abo} alt="or"></img>
            </div>
            <input className="items input inputform" name="email" type="email" placeholder="Введіть ваш логін"
                   size="30"/>
            <input className="items input inputform" name="password" type="password"
                   placeholder="Введіть ваш пароль"
                   size="30"/>
            <input className="items button" name="enterbutton" type="submit" value="Авторизуватися"/>
            <a href="/register" className="items rout">
                Ще немає профілю?
            </a>
            <a href="forgot" className="items rout">
                Забули пароль?
            </a>
        </form>
    )
}

export default Authorization;
