import React from 'react';
//import googleicon from '../../GoogleAuth.svg';
//import abo from '../../or.svg';
import { useFormik } from 'formik';
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
      <label htmlFor="email">Email Address</label>
      <input
        id="email"
        name="email"
        type="email"
        onChange={formik.handleChange}
        value={formik.values.email}
      />
      <label htmlFor="email">Pass</label>
      <input
        id="password"
        name="password"
        type="password"
        onChange={formik.handleChange}
        value={formik.values.password}
      />
      <label htmlFor="email">Confirm</label>
      <input
        id="confirmPassword"
        name="confirmPassword"
        type="confirmPassword"
        onChange={formik.handleChange}
        value={formik.values.confirmPassword}
      />

      <button type="submit">Submit</button>
    </form>
    // <form className="item content">
    //     <div className="items image">
    //         <img src={googleicon} alt="google"></img>
    //     </div>
    //     <div className="items or">
    //         <img src={abo} alt="or"></img>
    //     </div>
    //     <input className="items input inputform" name="email" type="email" placeholder="Введіть ваш логін"
    //            size="30"/>
    //     <input className="items input inputform" name="password" type="password"
    //            placeholder="Введіть ваш пароль"
    //            size="30"/>
    //     <input className="items input inputform" name="confirmpassword" type="password"
    //            placeholder="Повторіть ваш пароль" size="30"/>
    //     <input className="items button" href="/login" name="enterbutton" type="button" value="Зареєструватися"/>
    //     <div className="items deal">
    //         Натискаючи цю кнопку, <br/>
    //         ви погоджуєтесь з Угодою Користувача
    //         та Політикою Конфіденційності
    //     </div>
    // </form>
  );
};

export default Register;
