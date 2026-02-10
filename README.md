# Unity Project - Action Point

Unity로 제작 중인 2D 클릭형 RPG 프로젝트입니다.

각 층의 보스를 무찌르며 층을 오르는 구조를 가진 개인 학습용 프로젝트입니다.

## 프로젝트 개요
Unity 엔진 및 UI 구성, 행동별 결과 처리 등 게임 개발시 필요한 역량을 늘리기 위해 제작한 프로젝트입니다.

유튜브 강좌에서 학습한 내용 및 에셋을 활용하여 제작하였습니다.

## 구현 기능

### 1) 전투 시스템
- 층 오르기 행동 선택 -> 전투 진입 -> 종료 흐름 구성
- 10층 보스 처치 시 승리 처리

### 2) UI
1. 장비 제작 행동 시 비교 UI
2. 경고 UI(더 낮은 장비 착용 시, 체력이 50% 이하인 경우에 층을 오르려 할 때, 행동 포인트가 남아 있는 경우에 하루를 넘기려 할 때)
3. 전투 결과 UI(10층 보스 격파 혹은 패배)

### 3) 카메라 전환
- MainCamera / BattleCamera 구성
- 전투 진입 시 MainCamera 비활성화, BattleCamera 활성화
  
### 4) 사운드
- Bgm, 체력 회복, 전투(피격,죽음), 장비 제작

# 태우
# Action Point - Unity 2D Tower RPG

<p align="center"> <strong>행동 포인트 기반 턴제 전략 타워 클라이밍 RPG</strong> </p>

Unity로 개발한 2D 클릭형 RPG 게임입니다. 제한된 행동 포인트를 전략적으로 사용하여 10층 타워를 정복하는 것이 목표입니다.

## 목차

1. [게임 소개](#%EA%B2%8C%EC%9E%84-%EC%86%8C%EA%B0%9C)
2. [개발 환경](#%EA%B0%9C%EB%B0%9C-%ED%99%98%EA%B2%BD)
3. [설치 및 실행](#%EC%84%A4%EC%B9%98-%EB%B0%8F-%EC%8B%A4%ED%96%89)
4. [프로젝트 구조](#%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8-%EA%B5%AC%EC%A1%B0)
5. [핵심 시스템](#%ED%95%B5%EC%8B%AC-%EC%8B%9C%EC%8A%A4%ED%85%9C)
6. [데이터 구조](#%EB%8D%B0%EC%9D%B4%ED%84%B0-%EA%B5%AC%EC%A1%B0)
7. [게임플레이 매커니즘](#%EA%B2%8C%EC%9E%84%ED%94%8C%EB%A0%88%EC%9D%B4-%EB%A7%A4%EC%BB%A4%EB%8B%88%EC%A6%98)
8. [라이선스](#%EB%9D%BC%EC%9D%B4%EC%84%A0%EC%8A%A4)

---

## 게임 소개

**Action Point**는 제한된 자원(행동 포인트, 일수)을 효율적으로 관리하며 타워를 클리어하는 전략 RPG입니다.

### 핵심 컨셉

- **행동 포인트 시스템**: 하루에 3개의 행동 포인트를 사용하여 전투, 장비 제작, 체력 회복 중 선택
- **20일 제한**: 20일 안에 10층 보스를 처치하지 못하면 게임 오버
- **장비 크래프팅**: 층이 높아질수록 고등급 장비 획득 확률 증가
- **리스크 관리**: 체력이 낮을 때 전투 진입 시 경고 시스템

---

## 개발 환경

|항목|버전/사양|
|---|---|
|**Unity**|2022.x 이상 (2D URP 템플릿)|
|**렌더 파이프라인**|Universal Render Pipeline (URP)|
|**스크립팅 언어**|C#|
|**기본 해상도**|1920 x 1080|
|**입력 시스템**|New Input System|

### 사용된 Unity 패키지

- Universal RP
- TextMesh Pro
- Input System

---

## 설치 및 실행

### 요구 사항

- Unity 2022.3 LTS 이상
- Git

### 설치 방법

bash

```bash
# 1. 레포지토리 클론
git clone https://github.com/paul1405dev-hub/unity-project-actionpoint.git

# 2. Unity Hub에서 프로젝트 열기
# Unity Hub → Open → 클론한 폴더 선택
```

### 프로젝트 설정

1. Unity Hub에서 프로젝트를 열면 자동으로 필요한 패키지가 설치됩니다
2. `Assets/Scenes/SampleScene.unity`를 열어 게임을 실행합니다
3. Play 버튼을 눌러 게임을 테스트합니다

---

## 프로젝트 구조

```
Assets/
├── Project Action Point/
│   └── Codes/                    # 핵심 게임 스크립트
│       ├── GameManager.cs        # 게임 상태 관리 (싱글톤)
│       ├── ActionManager.cs      # 행동 포인트 시스템
│       ├── PlayerStats.cs        # 플레이어 스탯 및 전투 로직
│       ├── BattleController.cs   # 전투 씬 제어
│       ├── CraftingManager.cs    # 장비 제작 시스템
│       ├── Equipment.cs          # 장비 장착/해제
│       ├── GameViewManager.cs    # UI 및 카메라 전환
│       ├── AudioManager.cs       # 사운드 관리
│       ├── MonsterData.cs        # 몬스터 데이터 정의 (SO)
│       ├── ItemData.cs           # 아이템 데이터 정의 (SO)
│       ├── HUD.cs                # HUD 업데이트
│       └── FloatingText.cs       # 플로팅 텍스트 UI
├── Scenes/                       # 게임 씬
├── Fonts/                        # 폰트 리소스
├── Settings/                     # URP 설정
└── TextMesh Pro/                 # TMP 리소스
```

---

## 핵심 시스템

### 1. GameManager (게임 상태 관리)

싱글톤 패턴으로 구현된 중앙 관리자입니다.

csharp

```csharp
// 주요 속성
public int day = 1;      // 현재 날짜 (최대 20일)
public int floor = 0;    // 현재 층 (목표: 10층)
```

**주요 기능**:

- 게임 전역 상태 관리
- 날짜 진행 및 게임 오버 조건 체크
- 승리/패배 처리

### 2. ActionManager (행동 시스템)

하루에 사용 가능한 행동을 관리합니다.

csharp

```csharp
public enum ActionType {
    Battle,      // 층 오르기 (전투)
    BuildItem,   // 장비 제작
    RegenHealth, // 체력 회복 (+10)
    Sleep        // 수면 (다음 날로)
}

public int MaxActionPoint = 3;   // 하루 최대 행동 포인트
public int CurActionPoint;       // 현재 남은 행동 포인트
```

**경고 시스템**:

- 체력 50% 미만에서 전투 진입 시 경고
- 행동 포인트가 남아있는 상태에서 수면 시 경고

### 3. PlayerStats (전투 시스템)

코루틴 기반 턴제 전투 시뮬레이션을 담당합니다.

csharp

```csharp
public float MaxHealth = 100;    // 최대 체력
public float AttackPow = 10;     // 공격력 (장비로 증가)
public float DefensePow = 10;    // 방어력 (장비로 증가)
```

**전투 흐름**:

1. 층에 맞는 몬스터 선택 (10층은 보스 고정)
2. 전투 화면으로 전환
3. 턴제 공격 교환 (플레이어 → 몬스터)
4. 데미지 계산: `Max(1, 공격력 - 상대방어력)`
5. 승패 결과 처리

### 4. CraftingManager (제작 시스템)

층별 확률 테이블 기반 장비 제작 시스템입니다.

csharp

```csharp
public struct QualityProbability {
    public int floorThreshold;   // 적용 시작 층
    public int normalChance;     // 일반 등급 확률 (%)
    public int rareChance;       // 희귀 등급 확률 (%)
    // Epic = 100 - normal - rare
}
```

**제작 프로세스**:

1. 현재 층에 맞는 확률 테이블 적용
2. 등급 결정 (Normal → Rare → Epic)
3. 슬롯 타입 결정 (무기/방패)
4. 조건에 맞는 아이템 중 랜덤 선택
5. 현재 장비와 비교 UI 표시
6. 낮은 등급 장착 시 추가 경고

### 5. BattleController (전투 연출)

몬스터 스폰 및 애니메이션을 제어합니다.

**애니메이션 상태**:

- `Idle`: 대기 상태
- `Hit`: 피격 시 재생
- `Dead`: 사망 시 재생

### 6. AudioManager (사운드 시스템)

싱글톤 패턴의 오디오 관리자입니다.

csharp

```csharp
public enum Sfx {
    Craft,   // 장비 제작
    Health,  // 체력 회복
    Hit,     // 공격 피격
    Dead,    // 몬스터 사망
    Win,     // 게임 승리
    Lose,    // 게임 패배
    Climb    // 층 오르기
}
```

---

## 데이터 구조

### MonsterData (ScriptableObject)

csharp

```csharp
[CreateAssetMenu(fileName = "Monster", menuName = "Scriptble Object/MonsterData")]
public class MonsterData : ScriptableObject {
    public string monsterName;           // 몬스터 이름
    public bool isBoss;                  // 보스 여부 (10층 전용)
    public GameObject monsterPrefab;     // 프리팹
    public AnimatorOverrideController animatorOverride;  // 애니메이션

    public float maxHealth;              // 최대 체력
    public float attackPower;            // 공격력
    public float defensePower;           // 방어력
    public int floorToAppear;            // 등장 층
}
```

### ItemData (ScriptableObject)

csharp

```csharp
[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
public class ItemData : ScriptableObject {
    public enum SlotType { Weapon, Shield }
    public enum ItemQuality { Normal, Rare, Epic }

    public SlotType slotType;            // 장비 슬롯
    public ItemQuality quality;          // 등급
    public string itemName;              // 아이템 이름
    public Sprite itemIcon;              // 아이콘

    public float baseAttack;             // 기본 공격력
    public float baseDefense;            // 기본 방어력
}
```

---

## 게임플레이 매커니즘

### 승리 조건

- 20일 이내에 10층 보스 처치

### 패배 조건

- 전투 중 플레이어 체력 0 이하
- 20일 초과

### 행동 선택지

|행동|효과|비용|
|---|---|---|
|층 오르기|해당 층 몬스터와 전투|1 AP|
|장비 제작|랜덤 장비 획득|1 AP|
|체력 회복|HP +10 회복|1 AP|
|수면|다음 날로 진행, AP 3으로 회복|전체 AP|

### 장비 등급 시스템

|등급|색상|획득 확률|
|---|---|---|
|Normal|회색|층에 따라 변동|
|Rare|파랑|층에 따라 변동|
|Epic|보라|층에 따라 변동|

> 높은 층일수록 고등급 장비 획득 확률이 증가합니다.

---

## 확장 가이드

### 새로운 몬스터 추가

1. `Project` 창에서 우클릭 → `Create` → `Scriptble Object` → `MonsterData`
2. 몬스터 정보 입력 (이름, 스탯, 등장 층, 프리팹)
3. `PlayerStats` 컴포넌트의 `allMonsters` 배열에 추가

### 새로운 아이템 추가

1. `Project` 창에서 우클릭 → `Create` → `Scriptble Object` → `ItemData`
2. 아이템 정보 입력 (이름, 슬롯, 등급, 스탯)
3. `CraftingManager` 컴포넌트의 `itemTemplates` 배열에 추가

---

## 기술적 특징

- **싱글톤 패턴**: GameManager, AudioManager에서 사용하여 전역 접근 보장
- **ScriptableObject 활용**: 몬스터/아이템 데이터의 재사용성과 에디터 편집성 확보
- **코루틴 기반 전투**: 비동기적 전투 연출 및 타이밍 제어
- **이벤트 기반 UI**: 버튼 클릭과 게임 로직의 분리

---

## 라이선스

이 프로젝트는 개인 학습 목적으로 제작되었습니다. 사용된 에셋의 라이선스는 각 에셋의 원본 라이선스를 따릅니다.

---

## 기여

버그 리포트나 개선 제안은 [Issues](https://github.com/paul1405dev-hub/unity-project-actionpoint/issues)에 등록해 주세요.

---

<p align="center"> 개발: paul1405dev-hub </p>
