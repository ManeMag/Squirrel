import React from 'react';
import icon1 from "../../telicon.svg";
import icon2 from "../../faceicon.svg";
import icon3 from "../../twiticon.svg";

const Footer = () => {
    return (
        <footer className="item footer">
            <div className="fitem mark">© Squirrel 2022</div>
            <div className="fitem contacts">
                <div className="contact">
                    Follow us:
                </div>
                <div className="contact">
                    <img src={icon1} alt="telegram"></img>
                </div>
                <div className="contact">
                    <img src={icon2} alt="facebook"></img>
                </div>
                <div className="contact">
                    <img src={icon3} alt="twitter"></img>
                </div>
            </div>
        </footer>
    )
}

export default Footer;
