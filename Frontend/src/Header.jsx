import React from "react";
import NavBar from "./NavBar";
import "./Header.css"

function Header(){
    return (
        <>
            <p align="center" className="bg bg-primary py-3 fs-3">Vendor Portal</p>
            <NavBar/>
        </>
    );
}

export default Header;