

export  function GetTaskStatus(start, end){
    const StartDate = new Date(start)
    const EndDate = new Date(end)
    const DateNow = new Date()

    if(DateNow < StartDate){
        return "Uusi"
    }else if(DateNow > StartDate && DateNow < EndDate){
        return "Keskeneräinen"
    }else{
        return "Vanhentunut"
    }


}

//Methods for marking the task and activity as done
export async function MarkTaskDone(taskid){
    const DoneDate = new Date().toISOString().slice(0, 19).replace("T", " ");
    const Id = taskid;

    const formDataToSend = {
        Id,
        DoneDate
        
    }
    console.log(formDataToSend)
    try {
        const response = await fetch(
          "http://localhost:5299/api/Task/MarkTaskDone",
          {
            method: "PUT",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(formDataToSend),
          }
        );
        window.location.reload();
      } catch (error) {
        console.error("Error adding activity:", error);
      }
}


export async function MarkActivityDone(taskid){
    const DoneDate = new Date().toISOString().slice(0, 19).replace("T", " ");
    const Id = taskid;

    const formDataToSend = {
        Id,
        DoneDate
        
    }
    console.log(formDataToSend)
    try {
        const response = await fetch(
          "http://localhost:5299/api/Task/MarkActivityDone",
          {
            method: "PUT",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(formDataToSend),
          }
        );
        window.location.reload();
 
      } catch (error) {
        console.error("Error adding activity:", error);
      }
}