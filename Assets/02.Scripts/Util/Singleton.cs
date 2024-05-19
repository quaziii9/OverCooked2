using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 싱글톤 인스턴스를 저장할 변수
    private static T instance;
    // 멀티스레드 환경에서 안전하게 접근하기 위한 락 오브젝트
    private static readonly object lockObject = new object();

    // 싱글톤 인스턴스에 접근하기 위한 프로퍼티
    public static T Instance
    {
        get
        {
            // 인스턴스가 없는 경우 생성
            if (instance == null)
            {
                // 락을 걸어 멀티스레드 환경에서도 안전하게 처리
                lock (lockObject)
                {
                    // 인스턴스가 아직도 없는 경우 FindObjectOfType을 통해 찾거나 새로 생성
                    if (instance == null)
                    {
                        instance = (T)FindObjectOfType(typeof(T));

                        // 인스턴스가 여전히 없는 경우 새 게임 오브젝트를 생성하여 인스턴스를 붙임
                        if (instance == null)
                        {
                            GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                            instance = obj.GetComponent<T>();
                        }
                    }
                }
            }
            return instance;
        }
    }

    // Awake 메서드 - 인스턴스를 초기화하고 파괴되지 않도록 설정
    protected virtual void Awake()
    {
        // 인스턴스가 없는 경우 현재 오브젝트를 인스턴스로 설정하고 파괴되지 않도록 함
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        // 인스턴스가 이미 있는 경우 자신을 파괴
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    // protected 생성자를 통해 외부에서 인스턴스 생성을 방지
    protected Singleton() { }
}
