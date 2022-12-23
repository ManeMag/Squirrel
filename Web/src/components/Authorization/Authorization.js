import React from 'react';
import googleicon from '../../GoogleAuth.svg';
import abo from '../../or.svg';
import {useFormik} from 'formik';
import axios from 'axios';

const Authorization = () => {
    const formik = useFormik({
        initialValues: {
            email: '',
            password: '',
        },
        onSubmit: (values) => {
            console.log(JSON.stringify(values, null, 2));

            let bodyFormData = new FormData();
            bodyFormData.append('Email', values.email);
            bodyFormData.append('Password', values.password);

            axios({
                method: 'post',
                url: `http://geneirodan.zapto.org:23451/api/account/authenticate`,
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
        <form className="item login" onSubmit={formik.handleSubmit}>
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
            <input className="items button" name="enterbutton" type="submit" value="Авторизуватися"/>
            <a href="/register" className="items rout">
                Ще немає профілю?
            </a>
            <a href="forgot" className="items rout">
                Забули пароль?
            </a>
        </form>
    );
    // <form className="item login" name="form">

    //     <input  name="email" type="email"
    //            size="30"/>
    //     <input className="items input inputform" name="password" type="password"
    //
    //            />
    //     <input className="items button" name="enterbutton" type="submit" value="Авторизуватися"/>

    // </form>

};

export default Authorization;
