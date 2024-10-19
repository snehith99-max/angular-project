import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmMailsentComponent } from './crm-smm-mailsent.component';

describe('CrmSmmMailsentComponent', () => {
  let component: CrmSmmMailsentComponent;
  let fixture: ComponentFixture<CrmSmmMailsentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmMailsentComponent]
    });
    fixture = TestBed.createComponent(CrmSmmMailsentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
