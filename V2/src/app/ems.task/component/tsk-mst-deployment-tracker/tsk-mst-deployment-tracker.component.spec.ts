import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskMstDeploymentTrackerComponent } from './tsk-mst-deployment-tracker.component';

describe('TskMstDeploymentTrackerComponent', () => {
  let component: TskMstDeploymentTrackerComponent;
  let fixture: ComponentFixture<TskMstDeploymentTrackerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskMstDeploymentTrackerComponent]
    });
    fixture = TestBed.createComponent(TskMstDeploymentTrackerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
