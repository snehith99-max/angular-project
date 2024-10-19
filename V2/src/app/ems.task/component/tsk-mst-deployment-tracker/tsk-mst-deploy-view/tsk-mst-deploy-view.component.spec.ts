import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskMstDeployViewComponent } from './tsk-mst-deploy-view.component';

describe('TskMstDeployViewComponent', () => {
  let component: TskMstDeployViewComponent;
  let fixture: ComponentFixture<TskMstDeployViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskMstDeployViewComponent]
    });
    fixture = TestBed.createComponent(TskMstDeployViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
