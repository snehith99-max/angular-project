import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskMstDeployEditComponent } from './tsk-mst-deploy-edit.component';

describe('TskMstDeployEditComponent', () => {
  let component: TskMstDeployEditComponent;
  let fixture: ComponentFixture<TskMstDeployEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskMstDeployEditComponent]
    });
    fixture = TestBed.createComponent(TskMstDeployEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
