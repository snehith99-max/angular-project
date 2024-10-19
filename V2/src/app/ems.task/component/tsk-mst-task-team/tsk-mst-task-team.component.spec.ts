import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskMstTaskTeamComponent } from './tsk-mst-task-team.component';

describe('TskMstTaskTeamComponent', () => {
  let component: TskMstTaskTeamComponent;
  let fixture: ComponentFixture<TskMstTaskTeamComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskMstTaskTeamComponent]
    });
    fixture = TestBed.createComponent(TskMstTaskTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
