import React from "react";
import './NavBar.css'
import { NavLink, useNavigate } from "react-router-dom";

function NavBar(){
const navigate = useNavigate();

 const logout = () =>{
    const storedToken = sessionStorage.getItem('token');
    const storedExpirationTime = sessionStorage.getItem('expirationTime');
    if (storedToken && storedExpirationTime) {
        if (Date.now() < parseInt(storedExpirationTime, 10)) {
            if(window.confirm('your token is active , still do you want to logout ?')){
                sessionStorage.removeItem('token');
                sessionStorage.removeItem('expirationTime');  
                navigate('/login');
            }
            else{
                alert('logout operation is aborted !!');
            }
        } 
        else {
            sessionStorage.removeItem('token');
            sessionStorage.removeItem('expirationTime');
            alert('LogIn again !!');
            navigate('/login');
        } 
    }
    else{
        alert('LogIn again !!');
        navigate('/login');
    }
 }   

    return (
        <>
            <ul>
                <li>
                    <NavLink to="/">Home</NavLink>
                </li>
                <li>
                    <NavLink to="/login">LogIn</NavLink>
                </li>                                
                <li>
                    <NavLink to="/head">CreateProject Head</NavLink>
                </li>
                <li>
                    <button className="btn btn-danger logout" onClick={logout}>Log Out</button>
                </li>                
            </ul>
        </>
    );
}

export default NavBar;

/*
                <li>
                    <NavLink to="/login">Login</NavLink>
                </li>                
                <li>
                    <NavLink to="/add">Add Region</NavLink>
                </li>
                <li>
                    <NavLink to="/fetch">Fetch Regions</NavLink>
                </li>
                <li>
                    <NavLink to="/update">Update</NavLink>
                </li>                
                <li>
                    <NavLink to="/delete">Delete</NavLink>
                </li>
 */