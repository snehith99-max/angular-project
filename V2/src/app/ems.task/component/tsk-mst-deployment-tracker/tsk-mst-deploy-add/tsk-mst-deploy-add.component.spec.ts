import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskMstDeployAddComponent } from './tsk-mst-deploy-add.component';

describe('TskMstDeployAddComponent', () => {
  let component: TskMstDeployAddComponent;
  let fixture: ComponentFixture<TskMstDeployAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskMstDeployAddComponent]
    });
    fixture = TestBed.createComponent(TskMstDeployAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
