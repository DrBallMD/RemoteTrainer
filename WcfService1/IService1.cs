using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {

        
        [OperationContract]
        List<AISdb.AISTask> getTasks();
        [OperationContract]
        ANeuralNetwork.ANetwork getNeuralNetwork(int id);
        [OperationContract]
        void setNetworkResults(int id, ANeuralNetwork.ANetwork net, float newError);
        [OperationContract]
        float getError(int id);
        [OperationContract]
        List<ANeuralNetwork.StudyData> getStudyData(int id);
        [OperationContract]
        void addStudyData(int id, List<ANeuralNetwork.StudyData> data);
        [OperationContract]
        void addTask(AISdb.AISTask tt, List<string> parameters);
        [OperationContract]
        void updateTask(AISdb.AISTask tt);
        [OperationContract]
        void deleteTask(int id);
        [OperationContract]
        byte[] GetAssembly();
        // TODO: Добавьте здесь операции служб
    }


    // Используйте контракт данных, как показано в примере ниже, чтобы добавить составные типы к операциям служб.
    
}
