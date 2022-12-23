import React from 'react';
import googleicon from '../../GoogleAuth.svg';
import abo from '../../or.svg';
import {useFormik} from 'formik';
import axios from 'axios';

const Register = () => {
    const formik = useFormik({
        initialValues: {
            email: '',
            password: '',
            confirmPassword: '',
        },
        onSubmit: (values) => {
            console.log(JSON.stringify(values, null, 2));
            let bodyFormData = new FormData();
            bodyFormData.append('Email', values.email);
            bodyFormData.append('Password', values.password);
            bodyFormData.append('ConfirmPassword', values.confirmPassword);

            axios({
                method: 'post',
                url: `http://geneirodan.zapto.org:23451/api/account/register?callbackUrl=${document.location.href}`,
                data: bodyFormData,
                headers: {
                    'Content-Type': 'multipart/form-data',
                    'Access-Control-Allow-Origin': '*',
                },
                withCredentials: true,
            })
                .then((response) => {
                    alert('Welcome!');
                })
                .catch((err) => {
                    alert('Something has gone wrong!');
                });
        },
    });

    return (
        <form className="item content" onSubmit={formik.handleSubmit}>
            <div className="items image">
                <img src={googleicon} alt="google"></img>
            </div>
            <div className="items or">
                <img src={abo} alt="or"></img>
            </div>
            <input
                id="email"
                name="email"
                type="email"
                onChange={formik.handleChange}
                value={formik.values.email}
                className="items input inputform"
                placeholder="Введіть ваш логін"
                size="30"
            />
            <input
                id="password"
                name="password"
                type="password"
                onChange={formik.handleChange}
                value={formik.values.password}
                className="items input inputform"
                placeholder="Введіть ваш пароль"
                size="30"
            />
            <input
                id="confirmPassword"
                name="confirmPassword"
                type="confirmPassword"
                onChange={formik.handleChange}
                value={formik.values.confirmPassword}
                className="items input inputform"
                placeholder="Повторіть ваш пароль"
                size="30"
            />
            <input className="items button" href="/login" name="enterbutton" type="button" value="Зареєструватися"/>
            <div className="items deal">
                Натискаючи цю кнопку, <br/>
                ви погоджуєтесь з Угодою Користувача
                та Політикою Конфіденційності
            </div>
        </form>



    );
};

export default Register;
