//import NavbarApp from "./Components/NavbarApp";
import RfpRfqForm from "./Components/RfpRfqForm";
import Sidebar from "./Components/Sidebar";
import Vendor from "./Components/Vendor";
import Projecthead from "./Components/Projecthead"
import {BrowserRouter as Router,Routes,Route}  from "react-router-dom"
import Project from "./Components/Project";
import NavbarApp from "./Components/NavbarApp";
//import CreateHead from "./CreateHead";

function App() {
  return (
    <>
    <div>
    <NavbarApp />
    </div>

    <div style={{ display:"flex" }}>
    <Router>
      <div >
        
     <Sidebar/>
     </div>
     <div className="center-pages">   
      <Routes>
        <Route exact path ="/Vendor" element={<Vendor/>}/> 
        <Route exact path ="/Projecthead" element={<Projecthead/>}/>
        <Route exact path ="/RfpRfqForm" element={<RfpRfqForm/>}/>
        <Route exact path ="/Project" element={<Project/>}/>

      </Routes>
      </div>
      </Router>
      </div>
     
    </>
  );
}

export default App;
