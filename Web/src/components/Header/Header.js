import React from 'react';
import logo from "../../Squirrel.svg";

const Header = (props) => {
    return (
        <header className="item header">
            <div className="hitem logo">
                <div className="logoimg">
                    <a href='/login'><img src={logo} alt="logo"  ></img></a>
                </div>
            </div>
            <div className="hitem tname">
                <div className="htitle">
                    {window.location.pathname === '/login' ? 'Авторизація' : 'Реєстрація'}
                </div>
            </div>
            <div className="hitem navmenu"></div>
        </header>
    )
}

export default Header;
