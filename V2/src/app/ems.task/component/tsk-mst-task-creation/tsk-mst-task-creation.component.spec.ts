import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskMstTaskCreationComponent } from './tsk-mst-task-creation.component';

describe('TskMstTaskCreationComponent', () => {
  let component: TskMstTaskCreationComponent;
  let fixture: ComponentFixture<TskMstTaskCreationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskMstTaskCreationComponent]
    });
    fixture = TestBed.createComponent(TskMstTaskCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
