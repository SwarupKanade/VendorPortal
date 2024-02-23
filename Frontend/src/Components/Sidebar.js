
import './NavApp';


function Sidebar(){
    //const [show,setShow]=useState(false);
    return(
        <div >
        <div className="background"></div>
          <div className="sideBar">
            <ul>
                <li>
                    <div className='operation'>Create Vendor Account</div>
                </li>
                <li>
                    <div className='operation'>Create Project Head</div>
                </li>
                <li>
                
                    <div className='operation'>Create Project</div>
                </li>
                <li>
                    <div className='operation'>Create Request for Proposal</div>
                </li>
            </ul>
          </div>
        </div>
    );
}
export default Sidebar;