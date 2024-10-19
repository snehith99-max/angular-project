import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmMailsinboxComponent } from './crm-smm-mailsinbox.component';

describe('CrmSmmMailsinboxComponent', () => {
  let component: CrmSmmMailsinboxComponent;
  let fixture: ComponentFixture<CrmSmmMailsinboxComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmMailsinboxComponent]
    });
    fixture = TestBed.createComponent(CrmSmmMailsinboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
