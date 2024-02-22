
import './NavbarApp';


function Sidebar(){
    //const [show,setShow]=useState(false);
    return(
        <div >
        <div className="background"></div>
          <div className="sideBar">
            <ul>
                <li>
                    <h2>Create Vendor Account</h2>
                </li>
                <li>
                    
                    <h2>Create Project Head</h2>
                </li>
                <li>
                
                    <h2>Create Project</h2>
                </li>
                <li>
                    
                    <h2>Create Request for Proposal</h2>
                </li>
            </ul>
          </div>
        </div>
    );
}
export default Sidebar;