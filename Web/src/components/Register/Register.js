import React from 'react';
import googleicon from "../../GoogleAuth.svg";
import abo from "../../or.svg";

const Register = () => {
    return (
        <form className="item content">
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
            <input className="items input inputform" name="confirmpassword" type="password"
                   placeholder="Повторіть ваш пароль" size="30"/>
            <input className="items button" href="/login" name="enterbutton" type="button" value="Зареєструватися"/>
            <div className="items deal">
                Натискаючи цю кнопку, <br/>
                ви погоджуєтесь з Угодою Користувача
                та Політикою Конфіденційності
            </div>
        </form>
    )
}

export default Register;
